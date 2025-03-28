import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { KeycloakService } from 'keycloak-angular';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment.development';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  constructor(private keycloakService: KeycloakService, private http: HttpClient) {
  }
  
  async isLoggedIn() {
    return this.keycloakService.isLoggedIn();
  }

  async login() {
      this.keycloakService.login();
    }
  
    async logout() {
      this.keycloakService.logout();
    }
  
    getUserInfo() : Observable<any> {
      const token =  this.keycloakService.getToken(); // Get the current token
      return this.http.get(environment.keyCloak.userInfo, {
        headers: { Authorization: `Bearer ${token}` }
      })
    }

   async getToken() {
      return await this.keycloakService.getToken(); // Get the current token
    }
}
