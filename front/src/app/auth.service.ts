import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, Observable } from 'rxjs';
import { User } from './types/user';
import { UserClaim } from './types/user.claim';

function getClaimValue(claims: { type: string, value: string }[], claimType: string): string {
  const claim = claims.find(c => c.type === claimType);
  return claim ? claim.value : '';
}

function convertClaimsToUser(claims: UserClaim[]): User | null {

  if(claims == null){
    return null;
  }

  if(claims.length == 0){
    return null;
  }
  const user: User = {
    preferred_username: getClaimValue(claims, "preferred_username"),
    email: getClaimValue(claims, "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress"),
    given_name:  getClaimValue(claims, "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname"),
    family_name: getClaimValue(claims, "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/surname"),
  };

  return user;
}

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  constructor(private http: HttpClient) {}
 
  getUser() : Observable<User | null> {
     return this.getUserClaims().pipe(map(value => convertClaimsToUser(value)));
    }

  private getUserClaims(): Observable<UserClaim[]> {
    const httpOptions = {
      headers: new HttpHeaders({
        'X-CSRF': '1',
      }),
    };
   return this.http.get<UserClaim[]>(
      '/account/user?slide=false',
      httpOptions
    );
  }

  login() {
    window.location.href = `/account/login`;
  }

  logout() {
    this.getUserClaims().subscribe(value => {
      const logoutUrl = value?.find((claim) => claim.type === "bff:logout_url");
      if(logoutUrl != null) {
        window.location.href = logoutUrl.value;
      }
    });
  }
}
