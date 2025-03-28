import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment.development';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root'
})
export class ApiService {

  constructor(private http: HttpClient,private authService: AuthService ) {
  }
  
   manage(): Observable<any> {
     const token = this.authService.getToken();
      return this.http.get(`${environment.apiURL}/manage`, {
        headers: { Authorization: `Bearer ${token}` }
      })
    }

    read(): Observable<any> {
      const token = this.authService.getToken();
       return this.http.get(`${environment.apiURL}/read`, {
         headers: { Authorization: `Bearer ${token}` }
       })
     }

}
