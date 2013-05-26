﻿namespace System.Web.Mvc
{
    using System;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;

    /// <summary>
    ///     Расширение Loginza для представления
    /// </summary>
    public static class HelpersLoginza
    {
        /// <summary>
        ///     Инициализация, помещается в представлении в тэг head
        /// </summary>
        public static MvcHtmlString LoginzaInit(this HtmlHelper html)
        {
            return new MvcHtmlString(@"<script src=""//loginza.ru/js/widget.js"" type=""text/javascript""></script>");
        }

        public static MvcHtmlString Loginza(this HtmlHelper html)
        {
            return html.Loginza(null, null, (string[]) null);
        }

        /// <summary>
        ///     Получает виджет Loginza
        /// </summary>
        /// <param name="html"></param>
        /// <param name="action">Действие</param>
        /// <param name="controller">Контроллер</param>
        /// <returns></returns>
        public static MvcHtmlString Loginza(this HtmlHelper html, string action, string controller)
        {
            return html.Loginza(action, controller, null, (string[]) null);
        }


        /// <summary>
        ///     Получает виджет Loginza
        /// </summary>
        /// <param name="html"></param>
        /// <param name="providers">Список сервисов с которыми работать (через запятую, например "twitter,facebook")</param>
        /// <returns></returns>
        public static MvcHtmlString Loginza(this HtmlHelper html, params string[] providers)
        {
            return html.Loginza(null, null, providers);
        }

        /// <summary>
        ///     Получает виджет Loginza
        /// </summary>
        /// <param name="html"></param>
        /// <param name="textLink">Описание ссылки (может быть html)</param>
        /// <returns></returns>
        public static MvcHtmlString Loginza(this HtmlHelper html, string textLink)
        {
            return html.Loginza(null, textLink, (string[]) null);
        }

        /// <summary>
        ///     Получает виджет Loginza
        /// </summary>
        /// <param name="html"></param>
        /// <param name="textLink">Описание ссылки (может быть html)</param>
        /// <param name="providers">Список сервисов с которыми работать (через запятую, например "twitter,facebook")</param>
        /// <returns></returns>
        public static MvcHtmlString Loginza(this HtmlHelper html, string textLink, params string[] providers)
        {
            if (providers == null) throw new ArgumentNullException("providers");

            return html.Loginza(null, textLink, providers);
        }

        /// <summary>
        ///     Получает виджет Loginza
        /// </summary>
        /// <param name="html"></param>
        /// <param name="action">Действие</param>
        /// <param name="controller">Контроллер</param>
        /// <param name="textLink">Описание ссылки (может быть html)</param>
        /// <returns></returns>
        public static MvcHtmlString Loginza(this HtmlHelper html, string action, string controller, string textLink)
        {
            return html.Loginza(action, controller, textLink, (string[]) null);
        }

        /// <summary>
        ///     Получает виджет Loginza
        /// </summary>
        /// <param name="html"></param>
        /// <param name="action">Действие</param>
        /// <param name="controller">Контроллер</param>
        /// <param name="textLink">Описание ссылки (может быть html)</param>
        /// <param name="providers">Список сервисов с которыми работать (через запятую, например "twitter,facebook")</param>
        /// <returns></returns>
        public static MvcHtmlString Loginza(this HtmlHelper html, string action, string controller, string textLink, params string[] providers)
        {
            return html.Loginza(string.Format("/{1}/{0}", action, controller), textLink, providers);
        }

        /// <summary>
        ///     Получает виджет Loginza
        /// </summary>
        /// <param name="html"></param>
        /// <param name="uri">относительный URL на обработчик ответа сервиса</param>
        /// <param name="textLink">Описание ссылки (может быть html)</param>
        /// <param name="providers">Список сервисов с которыми работать (через запятую, например "twitter,facebook")</param>
        /// <returns></returns>
        public static MvcHtmlString Loginza(this HtmlHelper html, string uri, string textLink, params string[] providers)
        {
            if (string.IsNullOrEmpty(uri) == false && uri.Split(new[] {"/"}, StringSplitOptions.RemoveEmptyEntries).Length < 2)
            {
                return html.Loginza(string.Format("/{1}/{0}", uri, textLink), string.Empty, providers);
            }

            uri = uri ?? "/services/loginza";

            textLink = string.IsNullOrEmpty(textLink) ? @"<img src=""http://loginza.ru/img/sign_in_button_gray.gif"" alt=""Войти через loginza""/>" : textLink;

            Uri url = html.ViewContext.HttpContext.Request.Url;
            uri = uri.Trim();
            uri = uri.StartsWith("/") ? uri : "/" + uri;

            string pProvider = (providers != null && providers.Length == 1 && providers[0].Split(new[] {","}, StringSplitOptions.RemoveEmptyEntries).Length == 1) ? string.Format(@"&provider={0}", providers[0]) : string.Empty;
            string pProvidersSet = (providers != null && providers.Length > 1 && string.IsNullOrEmpty(pProvider))
                                       ? @"&providers_set=" + providers.Aggregate(string.Empty, (a, b) =>
                                           {
                                               a += b + ((providers.Last().Equals(b) && a.Split(new[] {","}, StringSplitOptions.RemoveEmptyEntries).Length == providers.Length - 1) ? string.Empty : ",");
                                               return a;
                                           })
                                       : string.Empty;


            return new MvcHtmlString(
                string.Format(@"<a href=""https://www.loginza.ru/api/widget?token_url={0}{1}{2}"" class=""loginza"">{3}</a>",
                              HttpUtility.UrlEncode(string.Format("{0}://{1}{2}", url.Scheme, url.Authority, uri)),
                              pProvidersSet,
                              pProvider,
                              textLink)
                );
        }
    }
}