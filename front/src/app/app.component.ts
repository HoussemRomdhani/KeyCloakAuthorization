import { Component, OnInit } from '@angular/core';
import { AuthService } from './auth.service';
import { ApiService } from './api.service';
import { User } from './types/user';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html'
})
export class AppComponent implements OnInit {
  isLoggedIn: boolean | null = null;
  userInfo: User | null | undefined;
  apiResult: any;
  constructor(public authService: AuthService, private apiService: ApiService) {
  }

   ngOnInit() {
     this.authService.getUser().subscribe(value => {
      if(value != null){
        this.isLoggedIn = true;
        this.userInfo = value;
      }
     })
  }
    
   login() {
     this.authService.login();
  }

   logout() {
     this.authService.logout();
  }

  manage() {
    this.clean();
     this.apiService.manage().subscribe({
      next: (value) => {
        this.apiResult = {
          success : true, 
          data: value
        }
      },
      error: (error) => {
        this.apiResult = {
          success : false, 
          data: error
        }
      }
     } 
  )
  }

  read() {
    this.clean();
    this.apiService.read().subscribe({
      next: (value) => {
        this.apiResult = {
          success : true, 
          data: value
        }
      },
      error: (error) => {
        this.apiResult = {
          success : false, 
          data: error
        }
      }
     } 
    )
 }

  async getUserInfo() {
    this.clean();
    this.authService.getUser().subscribe(value => {
      if(value != null){
        this.isLoggedIn = true;
        this.userInfo = value;
      }
     })
  }

  clean() {
    this.apiResult = null;
    this.userInfo = null;
  }
}

