// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace Toggl.iOS.Views
{
	[Register ("StartTimeEntryProjectsViewCell")]
	partial class ProjectSuggestionViewCell
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UILabel AmountOfTasksLabel { get; set; }

		[Outlet]
		UIKit.UIView BottomSeparatorView { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UILabel ClientNameLabel { get; set; }

		[Outlet]
		FadeView FadeView { get; set; }

		[Outlet]
		UIKit.NSLayoutConstraint FadeViewTrailingConstraint { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UIView ProjectDotView { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UILabel ProjectNameLabel { get; set; }

		[Outlet]
		UIKit.UIView SelectedProjectView { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UIImageView ToggleTaskImage { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UIButton ToggleTasksButton { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UIView TopSeparatorView { get; set; }

		void ReleaseDesignerOutlets ()
		{
			if (AmountOfTasksLabel != null) {
				AmountOfTasksLabel.Dispose ();
				AmountOfTasksLabel = null;
			}

			if (BottomSeparatorView != null) {
				BottomSeparatorView.Dispose ();
				BottomSeparatorView = null;
			}

			if (ClientNameLabel != null) {
				ClientNameLabel.Dispose ();
				ClientNameLabel = null;
			}

			if (FadeView != null) {
				FadeView.Dispose ();
				FadeView = null;
			}

			if (FadeViewTrailingConstraint != null) {
				FadeViewTrailingConstraint.Dispose ();
				FadeViewTrailingConstraint = null;
			}

			if (ProjectDotView != null) {
				ProjectDotView.Dispose ();
				ProjectDotView = null;
			}

			if (ProjectNameLabel != null) {
				ProjectNameLabel.Dispose ();
				ProjectNameLabel = null;
			}

			if (SelectedProjectView != null) {
				SelectedProjectView.Dispose ();
				SelectedProjectView = null;
			}

			if (ToggleTaskImage != null) {
				ToggleTaskImage.Dispose ();
				ToggleTaskImage = null;
			}

			if (ToggleTasksButton != null) {
				ToggleTasksButton.Dispose ();
				ToggleTasksButton = null;
			}

			if (TopSeparatorView != null) {
				TopSeparatorView.Dispose ();
				TopSeparatorView = null;
			}
		}
	}
}
