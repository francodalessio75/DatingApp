import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Member } from '../_models/member';

//insert an header containing the token for authorization purpose
const httpOptions = {
  headers: new HttpHeaders({
    Auhorization: 'Bearer ' + JSON.parse(localStorage.getItem('user'))?.token
  })
}

@Injectable({
  providedIn: 'root'
})
export class MembersService {
  baseUrl = environment.apiUrl;

  constructor( private http: HttpClient) { }

  getMenbers() : Observable<Member[]>{
    return this.http.get<Member[]>( this.baseUrl + 'users', httpOptions );
  }

  getMember(username:string){
    return this.http.get<Member>( this.baseUrl + 'users/' + username, httpOptions );
  }
}
