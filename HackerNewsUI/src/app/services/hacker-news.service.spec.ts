import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { HackerNewsService } from './hacker-news.service';
import { StoryQueryParameters, PaginatedResult, HackerNewsStory } from '../models/hacker-news.models';

describe('HackerNewsService', () => {
  let service: HackerNewsService;
  let httpMock: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [HackerNewsService]
    });
    service = TestBed.inject(HackerNewsService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  describe('getNewestStories', () => {
    it('should return a paginated result of stories', () => {
      const mockParams: StoryQueryParameters = { page: 1, pageSize: 10, search: 'test' };
      const mockResponse: PaginatedResult<HackerNewsStory> = {
        items: [{
          id: 1, title: 'Test Story', url: 'http://test.com', by: 'author', time: 12345, score: 100, descendants: 5,
          type: 'story', timeStamp: new Date(), hasUrl: true
        }],
        pageNumber: 1,
        pageSize: 10,
        totalCount: 1,
        totalPages: 1,
        hasPrevious: false,
        hasNext: false
      };

      service.getNewestStories(mockParams).subscribe(result => {
        expect(result.items.length).toBe(1);
        expect(result.items[0].title).toBe('Test Story');
      });

      const req = httpMock.expectOne(`${service.apiUrl}/newest?page=1&pageSize=10&search=test`);
      expect(req.request.method).toBe('GET');
      req.flush(mockResponse);
    });
  });

  describe('checkApiHealth', () => {
    it('should return a health status', () => {
      const mockHealth = { status: 'healthy' };

      service.checkApiHealth().subscribe(result => {
        expect(result.status).toBe('healthy');
      });

      const req = httpMock.expectOne(`${service.apiUrl}/health`);
      expect(req.request.method).toBe('GET');
      req.flush(mockHealth);
    });
  });
});
