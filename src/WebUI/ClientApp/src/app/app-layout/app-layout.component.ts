import { Component, OnInit } from '@angular/core';
import { NavigationCancel, NavigationEnd, NavigationError, NavigationStart, Router, RouterEvent } from '@angular/router';

declare var cordova;

@Component({
  selector: 'app-app-layout',
  templateUrl: './app-layout.component.html',
  styleUrls: ['./app-layout.component.css']
})
export class AppLayoutComponent implements OnInit {

  public showSpinner = true;

  constructor(
    private router: Router
  ) {
    router.events.subscribe((event: RouterEvent) => {
      this.navigationInterceptor(event);
    });
    
  }

  ngOnInit(): void {

    document.addEventListener('deviceready', () => {
      //alert(cordova);



      if (cordova) {
        //window.open = cordova.InAppBrowser.open;
        const x = `https://rentasguatemala.com/connect/authorize?client_id=rentasgt.MobileApp&redirect_uri=com.rentasguatemala%3A%2F%2Foauth_callback&scope=openid%20profile%20rentasgt.WebUIAPI&response_type=token%20id_token&response_mode=fragment&nonce=2d7uka4bmsh&code_challenge=GoZEF_M35jouQEHVgVOTkk9-fBrjMkCjMvUOW8rydOA&code_challenge_method=S256`;
        // const x = `https://rentasguatemala.com/Identity/Account/Login?ReturnUrl=%2Fconnect%2Fauthorize%2Fcallback%3Fclient_id%3Drentasgt.WebUI%26redirect_uri%3Dhttps%253A%252F%252Frentasguatemala.com%252Fauthentication%252Flogin-callback%26response_type%3Dcode%26scope%3Drentasgt.WebUIAPI%2520openid%2520profile%26state%3Dccbe30f0f24f45cb919d1851282eb0f1%26code_challenge%3DP2xsV3NAKMZbzBeV7PCQ07CiQNwVLYxsKpZ5ZHqmGfQ%26code_challenge_method%3DS256%26response_mode%3Dquery`;
        // const ref = window.open(x, '_blank', 'location=no,hideurlbar=yes,clearcache=yes,clearsessioncache=yes,zoom=no,cleardata=yes' );
        (window as any).handleOpenURL = (url: string) => {
          alert(url);
        };
        window.open(x, 'com.rentasguatemala:oauth');
        // ref.addEventListener('loadstart', (event: any) => {
        //   console.log(event.url);
        //   if (event.url.startsWith("https://rentasguatemala.com/connect/authorize/callback")) {
        //        console.log(event.url);
        //        alert(event.url);
        //        ref.close();
        //   }
        // });
        // ref.addEventListener('loadstop', (event: any) => {
        //   console.log(event.url);
        //   if (event.url.startsWith("com.rentasguatemala://oauth_callback")) {
        //        ref.close();
        //   }
        // });
        // ref.addEventListener('loaderror', () => {
          
        // });
    
        // window.addEventListener('message', function (event) {
        //   alert("Mensaje!");
        //   if (event.data.match(/^oauth::/)) {
        //     var data = JSON.parse(event.data.substring(7));
        //     alert(data);
        //   }
        //   window.alert(event.data);
        // });
      }
    }, false);
    
    
    // const x = `https://rentasguatemala.com/connect/authorize?client_id=rentasgt.MobileApp&redirect_uri=com.rentasguatemala%3A%2F%2Foauth_callback&scope=openid%20profile%20rentasgt.WebUIAPI&response_type=token%20id_token&response_mode=fragment&nonce=2d7uka4bmsh&code_challenge=GoZEF_M35jouQEHVgVOTkk9-fBrjMkCjMvUOW8rydOA&code_challenge_method=S256`;
    // window.open(x, 'oauth:rentas_login');

    // window.addEventListener('message', function (event) {
    //   alert("Mensaje!");
    //   if (event.data.match(/^oauth::/)) {
    //     var data = JSON.parse(event.data.substring(7));
    //     alert(data);
    //   }
    //   window.alert(event.data);
    // });

  }

  navigationInterceptor(event: RouterEvent): void {
    if (event instanceof NavigationStart) {
      this.showSpinner = true;
    }
    if (event instanceof NavigationEnd) {
      this.showSpinner = false;
    }

    // Set loading state to false in both of the below events to hide the spinner in case a request fails
    if (event instanceof NavigationCancel) {
      this.showSpinner = false;
    }
    if (event instanceof NavigationError) {
      this.showSpinner = false;
    }
  }

}
