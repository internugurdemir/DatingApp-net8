import { HttpClient, HttpHeaders, HttpParams, HttpResponse } from '@angular/common/http';
import { Injectable, inject, model, signal } from '@angular/core';
import { environment } from '../../environments/environment';
import { Member } from '../_models/member';
import { of, tap } from 'rxjs';
// import { of, tap } from 'rxjs';
// import { Photo } from '../_models/photo';
// import { PaginatedResult } from '../_models/pagination';
// import { UserParams } from '../_models/userParams';
// import { setPaginatedResponse, setPaginationHeaders } from './paginationHelper';

@Injectable({
  providedIn: 'root'
})
export class MembersService {
  private http = inject(HttpClient);
  baseUrl = environment.apiUrl;
  members=signal<Member[]>([]);
  getMembers() {
    return this.http.get<Member[]>(this.baseUrl + 'users').subscribe({        
      next: members=> this.members.set(members)  
    });
  }  
  getMember(username: string) {
    const member = this.members().find(a=>a.username === username);
    if (member !== undefined) { return of(member);  }

    return this.http.get<Member>(this.baseUrl + 'users/'+username);
  }  
  updateMember(member: Member) {
    return this.http.put(this.baseUrl + 'users', member)
    .pipe(
      tap(() => {
        this.members.update(members => members.map(m => m.username === member.username 
            ? member : m))
      })
    )
  }

}
