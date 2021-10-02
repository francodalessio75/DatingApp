import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ReplaySubject } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { User } from '../_models/user';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  baseUrl = environment.apiUrl;
  /* special type of observable that is like a buffer and any tyime it is triggered it just emits
    all its las n stored values whare n is the parmater of its constructor.
    So here it returns the last one logged in user.*/
  private currentUserSource = new ReplaySubject<User>(1);
  /**
   * This observable is observed by authGuard component, that in turn returns an observable returning a
   * boolean to choose if a resource can be accessed or not
   */
  currentUser$ = this.currentUserSource.asObservable();

  constructor( private http : HttpClient) { }

  login(model:any){
    return this.http.post(this.baseUrl + 'account/login', model).pipe(
      map((user:User) => {
        if(user){
         this.setCurrentUser(user);
        }
      })
    )
  }

  register(model:any){
    return this.http.post(this.baseUrl + 'account/register', model).pipe(
      map( (user:User) => {
        if(user){

          this.currentUserSource.next(user);
        }
      })
    )
  }

  setCurrentUser( user:User ){
    localStorage.setItem('user', JSON.stringify(user));
    this.currentUserSource.next(user);
  }

  logout(){
    localStorage.removeItem('user');
    this.currentUserSource.next(null);
  }
}


