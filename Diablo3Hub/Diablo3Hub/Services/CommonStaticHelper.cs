﻿using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media;
using Microsoft.Toolkit.Uwp.UI.Extensions;
using ApplicationView = Windows.UI.ViewManagement.ApplicationView;

namespace Diablo3Hub.Services
{
    public static class CommonStaticHelper
    {
        /// <summary>
        /// 여기서 관리하는 팝업
        /// </summary>
        private static Popup _singlePopup;
        /// <summary>
        /// 관리하는 팝업의 취소 토큰 소스
        /// </summary>
        private static CancellationTokenSource _singlePopupCTS;
        /// <summary>
        /// 싱글 팝업 테스크 컴플레이션 소스
        /// </summary>
        private static TaskCompletionSource<object> _singlePopupTCS;

        /// <summary>
        ///     공통 확인 창 출력 메서드 입니다.
        ///     상세 내용은 아래 페이지 참고
        ///     https://www.reflectionit.nl/blog/2015/windows-10-xaml-tips-messagedialog-and-contentdialog
        /// </summary>
        /// <param name="message"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        public static async Task<object> ShowConfirmBoxAsync(string message, string title = null)
        {
            var dialog = title == null
                ? new MessageDialog(message)
                : new MessageDialog(message, title);
            dialog.Commands.Add(new UICommand("Yes") {Id = 1});
            dialog.Commands.Add(new UICommand("No") {Id = 0});

            dialog.DefaultCommandIndex = 0;
            dialog.CancelCommandIndex = 0;
            var result = await dialog.ShowAsync();
            return result.Id;
        }
        /// <summary>
        /// 간단한 메시지 박스 출력
        /// </summary>
        /// <param name="message"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        public static async Task<object> ShowMessageBoxAsync(string message, string title = null)
        {
            var dialog = title == null
                ? new MessageDialog(message)
                : new MessageDialog(message, title);

            var result = await dialog.ShowAsync();
            return result.Id;
        }
        /// <summary>
        /// 윈도우 바운드 반환
        /// </summary>
        /// <returns></returns>
        public static Rect GetWindowBounds()
        {
            var view = ApplicationView.GetForCurrentView();
            return view.VisibleBounds;
        }
        /// <summary>
        /// 싱글 팝업 출력
        /// </summary>
        public static Task<object> ShowPopupAsync(FrameworkElement content, string okText, string cancelText)
        {
            if (content == null) return null;
            if (double.IsNaN(content.Width) || double.IsNaN(content.Height)) return null;

            var rect = GetWindowBounds();
            //var hoffset = Math.Ceiling(rect.Width / 2 - content.Width / 2);
            //var voffset = Math.Ceiling(rect.Height / 2 - content.Height / 2);
            var border = new Border
            {
                Background = new SolidColorBrush(Color.FromArgb(40, 0, 0, 0)),
                Child = content,
                Width = rect.Width,
                Height = rect.Height
            };

            _singlePopup = new Popup
            {
                Child = border,
                IsOpen = true
            };
            _singlePopupTCS = new TaskCompletionSource<object>();
            return _singlePopupTCS.Task;
        }
        /// <summary>
        /// 팝업 완료
        /// </summary>
        /// <param name="result"></param>
        public static void SetResult(object result)
        {
            if (_singlePopup == null || _singlePopupTCS == null) return;
            _singlePopup.IsOpen = false;
            _singlePopupTCS.SetResult(result);
        }
        /// <summary>
        /// 팝업 취소
        /// </summary>
        public static void SetCancel()
        {
            if (_singlePopup == null || _singlePopupTCS == null) return;
            _singlePopup.IsOpen = false;
            _singlePopupTCS.SetCanceled();
        }
    }
}