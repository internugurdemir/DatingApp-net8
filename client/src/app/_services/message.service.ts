import { inject, Injectable, signal } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { PaginatedResult } from '../_models/pagination';
import { Message } from '../_models/message';
import { setPaginatedResponse, setPaginationHeaders } from './paginationHelper';

@Injectable({
  providedIn: 'root'
})
export class MessageService {
  baseUrl = environment.apiUrl;

  private http = inject(HttpClient);
  paginatedResult = signal<PaginatedResult<Message[]> | null>(null);
getMessages(pageNumber: number, pageSize: number, container: string) {
  let params = setPaginationHeaders(pageNumber, pageSize);
  params = params.append('Container', container);

  return this.http.get<Message[]>(this.baseUrl + 'messages', { observe: 'response', params })
    .subscribe({
      next: response => {
        // console.log('Tüm response:', response);
        // console.log('Mesaj listesi (body):', response.body);
        // console.log('Pagination header:', response.headers.get('Pagination'));
        setPaginatedResponse(response, this.paginatedResult)
      },
      error: err => {
        console.error('Hata oluştu:', err);
      }
    });
}

  getMessageThread(username: string) {
    return this.http.get<Message[]>(this.baseUrl + 'messages/thread/' + username);
  }

  sendMessage(username: string, content: string) {
    return this.http.post<Message>(this.baseUrl + 'messages', { RecipientUsername: username, content })
  }

    deleteMessage(id: number) {
    return this.http.delete(this.baseUrl + 'messages/' + id);
  }
}
