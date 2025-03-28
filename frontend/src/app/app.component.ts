import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { AuthService } from './auth.service';
import { ApiService } from './api.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'frontend';
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
    this.apiResult = null;
     this.apiService.manage().subscribe(value =>{
      this.apiResult = value;
    })
  }

  read() {
    this.apiResult = null;
    this.apiService.read().subscribe(value =>{
     this.apiResult = value;
   })
 }

  async getUserInfo() {
    this.authService.getUserInfo().subscribe(value => {
      this.userInfo = value;
    })
  }
}

