﻿using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using Toggl.Foundation.Exceptions;
using Toggl.Foundation.Interactors;
using Toggl.Foundation.MvvmCross.Collections;
using Toggl.Foundation.MvvmCross.ViewModels.Selectable;
using Toggl.Foundation.Services;
using Toggl.Multivac;
using Toggl.Multivac.Extensions;
using Toggl.PrimeRadiant.Settings;

namespace Toggl.Foundation.MvvmCross.ViewModels.Calendar
{
    public abstract class SelectUserCalendarsViewModelBase : MvxViewModel<bool, string[]>
    {
        protected readonly IUserPreferences UserPreferences;
        private readonly IInteractorFactory interactorFactory;
        private readonly IMvxNavigationService navigationService;
        private readonly IRxActionFactory rxActionFactory;

        private ISubject<IImmutableList<CollectionSection<UserCalendarSourceViewModel, SelectableUserCalendarViewModel>>> calendarsSubject =
            new BehaviorSubject<IImmutableList<CollectionSection<UserCalendarSourceViewModel, SelectableUserCalendarViewModel>>>(
                ImmutableList.Create<CollectionSection<UserCalendarSourceViewModel, SelectableUserCalendarViewModel>>());

        public IObservable<IImmutableList<CollectionSection<UserCalendarSourceViewModel, SelectableUserCalendarViewModel>>> Calendars { get; }

        public InputAction<SelectableUserCalendarViewModel> SelectCalendar { get; }
        public UIAction Close { get; private set; }
        public UIAction Done { get; private set; }

        protected bool ForceItemSelection { get; private set; }

        protected HashSet<string> InitialSelectedCalendarIds { get; } = new HashSet<string>();
        protected HashSet<string> SelectedCalendarIds { get; } = new HashSet<string>();

        protected SelectUserCalendarsViewModelBase(
            IUserPreferences userPreferences,
            IInteractorFactory interactorFactory,
            IMvxNavigationService navigationService,
            IRxActionFactory rxActionFactory)
        {
            Ensure.Argument.IsNotNull(userPreferences, nameof(userPreferences));
            Ensure.Argument.IsNotNull(interactorFactory, nameof(interactorFactory));
            Ensure.Argument.IsNotNull(navigationService, nameof(navigationService));
            Ensure.Argument.IsNotNull(rxActionFactory, nameof(rxActionFactory));

            this.UserPreferences = userPreferences;
            this.interactorFactory = interactorFactory;
            this.navigationService = navigationService;
            this.rxActionFactory = rxActionFactory;

            SelectCalendar = rxActionFactory.FromAction<SelectableUserCalendarViewModel>(toggleCalendarSelection);

            Calendars = calendarsSubject.AsObservable().DistinctUntilChanged();
        }

        public sealed override void Prepare(bool parameter)
        {
            ForceItemSelection = parameter;
        }

        public override async Task Initialize()
        {
            await base.Initialize();

            var calendarIds = UserPreferences.EnabledCalendarIds();
            InitialSelectedCalendarIds.AddRange(calendarIds);
            SelectedCalendarIds.AddRange(calendarIds);

            await ReloadCalendars();

            var enabledObservable = ForceItemSelection
                ? SelectCalendar.Elements
                    .Select(_ => SelectedCalendarIds.Any())
                    .DistinctUntilChanged()
                : Observable.Return(true);

            Close = rxActionFactory.FromAsync(close);
            Done = rxActionFactory.FromAsync(done, enabledObservable);
        }

        protected async Task ReloadCalendars()
        {
            var calendars = await interactorFactory
                .GetUserCalendars()
                .Execute()
                .Catch((NotAuthorizedException _) => Observable.Return(new List<UserCalendar>()))
                .Select(group);

            calendarsSubject.OnNext(calendars);
        }

        private IImmutableList<CollectionSection<UserCalendarSourceViewModel, SelectableUserCalendarViewModel>> group(IEnumerable<UserCalendar> calendars)
            => calendars
                .Select(toSelectable)
                .GroupBy(calendar => calendar.SourceName)
                .Select(group =>
                    new CollectionSection<UserCalendarSourceViewModel, SelectableUserCalendarViewModel>(
                        new UserCalendarSourceViewModel(group.First().SourceName),
                        group.OrderBy(calendar => calendar.Name)
                    )
                )
                .ToImmutableList();

        private SelectableUserCalendarViewModel toSelectable(UserCalendar calendar)
            => new SelectableUserCalendarViewModel(calendar, SelectedCalendarIds.Contains(calendar.Id));

        private void toggleCalendarSelection(SelectableUserCalendarViewModel calendar)
        {
            if (SelectedCalendarIds.Contains(calendar.Id))
                SelectedCalendarIds.Remove(calendar.Id);
            else
                SelectedCalendarIds.Add(calendar.Id);
            calendar.Selected = !calendar.Selected;
        }

        private Task close()
            => navigationService.Close(this, InitialSelectedCalendarIds.ToArray());

        private Task done()
            => navigationService.Close(this, SelectedCalendarIds.ToArray());
    }
}
