import { Component, OnInit } from '@angular/core';
import { AuthService } from './auth.service';
import { ApiService } from './api.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html'
})
export class AppComponent implements OnInit {
  isLoggedIn: boolean | null = null;
  userInfo: any;
  apiResult: any;
  constructor(private authService: AuthService, private apiService: ApiService) {
  }

  async ngOnInit() {
    this.isLoggedIn = await this.authService.isLoggedIn();
  }
    
  async login() {
    await this.authService.login();
  }

  async logout() {
    await this.authService.logout();
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
    this.authService.getUserInfo().subscribe(value => {
      this.userInfo = value;
    })
  }

  clean() {
    this.apiResult = null;
    this.userInfo = null;
  }
}

