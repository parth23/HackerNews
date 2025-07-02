import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { HackerNewsStory, PaginatedResult, StoryQueryParameters } from '../models/hacker-news.models';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class HackerNewsService {
  private apiUrl = `${environment.apiUrl}/stories`;

  constructor(private http: HttpClient) { }

  getNewestStories(params: StoryQueryParameters): Observable<PaginatedResult<HackerNewsStory>> {
    let httpParams = new HttpParams()
      .set('page', params.page.toString())
      .set('pageSize', params.pageSize.toString());

    if (params.search && params.search.trim()) {
      httpParams = httpParams.set('search', params.search.trim());
    }

    return this.http.get<PaginatedResult<HackerNewsStory>>(`${this.apiUrl}/newest`, { params: httpParams })
      .pipe(
        map(result => ({
          ...result,
          items: result.items.map(story => ({
            ...story,
            timeStamp: new Date(story.time * 1000), // Convert Unix timestamp to Date
            hasUrl: !!(story.url && story.url.trim())
          }))
        }))
      );
  }

  checkApiHealth(): Observable<any> {
    return this.http.get(`${this.apiUrl}/health`);
  }
}
