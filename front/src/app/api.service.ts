import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, Observable, of, throwError } from 'rxjs';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root'
})
export class ApiService {
  httpOptions = {
    headers: new HttpHeaders({
      'X-CSRF': '1',
    }),
  };
  
  constructor(private http: HttpClient,private authService: AuthService ) {
  }
  
   manage(): Observable<any> {
      return this.http.get('api/manage', this.httpOptions).pipe(catchError(this.handleError))
    }

    read(): Observable<any> {
       return this.http.get('api/read',this.httpOptions).pipe(catchError(this.handleError))
     }

     private handleError(error: HttpErrorResponse) {
       return throwError(() => error.message); 
    }

}
