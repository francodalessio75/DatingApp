import { HttpClient } from '@angular/common/http';
import { Injectable, OnInit } from '@angular/core';
import { of } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { Member } from '../_models/member';
import { Photo } from '../_models/photo';

@Injectable({
  providedIn: 'root'
})
export class MembersService {
  baseUrl = environment.apiUrl;
  members : Member[] = [];

  constructor( private http: HttpClient) { }

  getMembers(){
    if(this.members.length > 0 ){
      return of(this.members);
    }else{
      return this.http.get<Member[]>( this.baseUrl + 'users' ).pipe(
        map( members => {
          this.members = members;
          return members;
        })
      );
    }
  }

  getMember(username:string){

    const member = this.members.find(x => x.username === username);
    if( member!== undefined ){
      return of(member);
    } else{
      return this.http.get<Member>( this.baseUrl + 'users/' + username );
    }
  }

  updateMember(member : Member){
    return this.http.put<Member>( this.baseUrl + 'users', member).pipe(
      map(() => {
        const index = this.members.indexOf(member);
        this.members[index] = member;
      })
    );
  }

  setMainPhoto( photoId : number ){
    return this.http.put<Member>( this.baseUrl + 'users/set-main-photo/' + photoId, {});
  }

  deletePhoto(photoId: number){
    return this.http.delete(this.baseUrl + 'users/delete-photo/' + photoId);
  }
}
