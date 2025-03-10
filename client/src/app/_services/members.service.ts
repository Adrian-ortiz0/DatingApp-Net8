import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import { environment } from '../../environments/environment';
import { Member } from '../_models/member';
import { of, tap } from 'rxjs';
import { Photo } from '../_models/photo';
import { PaginationResult } from '../_models/pagination';

@Injectable({
  providedIn: 'root'
})
export class MembersService {
  private http = inject(HttpClient);
  baseUrl = environment.apiUrl;
  members = signal<Member[]>([]);
  paginatedResult = signal<PaginationResult<Member[]> | null>(null)
  
  getMembers(pageNumber?: number, pageSize?: number){
    let params = new HttpParams();
    if(pageNumber && pageSize){
      params = params.append("pageNumber", pageNumber);
      params = params.append("pageSize", pageSize)
    }
    return this.http.get<Member[]>(this.baseUrl + "users", {observe: 'response', params}).subscribe({
      next: (response) => {
       this.paginatedResult.set({
        items: response.body as Member[],
        pagination: JSON.parse(response.headers.get('Pagination')!)
       })
      }
    })
  }
  getMember(name: string){
    // const member = this.members().find(x => x.name === name);
    // if(member !== undefined){
    //   return of (member);
    // }

    return this.http.get<Member>(this.baseUrl + "users/" + name.toLowerCase());
  }
  updateMember(member: Member){
    return this.http.put(this.baseUrl + "users", member).pipe(
      // tap(() => {
      //   this.members.update(members => members.map(m => m.name === member.name 
      //     ? member : m))
      // })
    )
  }

  setMainPhoto(photo: Photo){
    return this.http.put( this.baseUrl + "users/set-main-photo/" + photo.id, {}).pipe(
      // tap(() => {
      //   this.members.update(members => members.map(m => {
      //     if(m.photos.includes(photo)){
      //       m.photoUrl = photo.url
      //     }
      //     return m;
      //   }))
      // })
    )
  }

  deletePhoto(photoId: number){
    return this.http.delete(this.baseUrl + 'users/delete-photo/' + photoId);
  }

}
