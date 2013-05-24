LoginzaProvider
===============

.NET provider for Loginza

NuGet
===============
https://nuget.org/packages/LoginzaProvider

Example for MVC application
===============

View
---------------------
```html
<div class="loginza">
<script src="http://loginza.ru/js/widget.js" type="text/javascript"></script>
<iframe src="http://loginza.ru/api/widget?overlay=loginza&token_url=@(Url.Action<AccountController>(x => x.RegisterByLoginza()))&style="width:330px;height:200px;" scrolling="no" frameborder="no"></iframe>
</div>
```

---------------------
```html
<html>
  <head>
    ....
    @Html.LoginzaInit()
    ....
 </head>
 <body>    
        <div class="container">
           ...
           <div class="top_panel">
                    @Html.Loginza()
            </div>
            ...
           <div class="left_panel">
                    @Html.Loginza("RegisterByLoginza","Account",new [] {"google","yandex","mailru","rambler"})
           </div>
            ...
            <div class="right_panel">
                    @Html.Loginza("Account/RegisterByLoginza","Social network",new [] {"vkontakte","odnoklassniki","facebook"})
            </div>
           ...
           <div class="bottom_panel">
                    @Html.Loginza("RegisterByLoginza", "Account", "<img src=\"\\Content\\images\\my_image\" alt=\"\"/>", "webmoney", "rambler", "flickr", "lastfm", "verisign", "aol")
            </div>
 </body>
</html>
```

ViewModel
--------------------
```c#
public class LoginzaRegistration
{
	public string Token { get; set; }
}
```

Controller
--------------------
```c#
public class AccountController : Controller
{
    [HttpPost]
  	public ActionResult RegisterByLoginza(LoginzaRegistration form)
		{
			if (string.IsNullOrEmpty(form.Token))
  		  		throw new Exception("Could not connect to Loginza service");

			var loginza = new LoginzaProvider(<LoginzaWidgetId>, <LoginzaSecureKey>);

			AuthenticationData data = loginza.GetAuthenticationData(form.Token);

			if (data.IsValid() == false)
				throw new Exception(string.Format("Could not connect to Loginza service. Reason: {0}. Error Message: {1}", data.Error_Type, data.Error_Message));
      
  		  // use data.Provider, data.Identity, data.Uid properties to identify user
		}
}
```