export interface HackerNewsStory {
  id: number;
  title?: string;
  url?: string;
  by?: string;
  time: number;
  score: number;
  descendants: number;
  type: string;
  kids?: number[];
  timeStamp: Date;
  hasUrl: boolean;
}

export interface PaginatedResult<T> {
  items: T[];
  totalCount: number;
  pageNumber: number;
  pageSize: number;
  totalPages: number;
  hasNext: boolean;
  hasPrevious: boolean;
}

export interface StoryQueryParameters {
  page: number;
  pageSize: number;
  search?: string;
}
