import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HackerNewsService } from '../../services/hacker-news.service';
import { HackerNewsStory, PaginatedResult, StoryQueryParameters } from '../../models/hacker-news.models';
import { StoryItemComponent } from '../story-item/story-item';
import { PaginationComponent } from '../pagination/pagination';
import { finalize } from 'rxjs/operators';

@Component({
  selector: 'app-stories',
  standalone: true,
  imports: [CommonModule, FormsModule, StoryItemComponent, PaginationComponent],
  templateUrl: './stories.html',
  styleUrls: ['./stories.scss']
})
export class StoriesComponent implements OnInit {
  stories: PaginatedResult<HackerNewsStory> | null = null;
  loading = false;
  error: string | null = null;
  searchTerm = '';
  currentPage = 1;
  pageSize = 20;

  constructor(private hackerNewsService: HackerNewsService, private cdr: ChangeDetectorRef) {}

  ngOnInit(): void {
    this.loadStories();
  }

  loadStories(): void {
    this.loading = true;
    this.error = null;

    const params: StoryQueryParameters = {
      page: this.currentPage,
      pageSize: this.pageSize,
      search: this.searchTerm
    };

    this.hackerNewsService.getNewestStories(params).pipe(
      finalize(() => {
        this.loading = false;
        this.cdr.detectChanges();
      })
    ).subscribe({
      next: (result) => {
        this.stories = result;
      },
      error: (error) => {
        console.error('Error loading stories:', error);
        this.error = 'Failed to load stories. Please try again.';
      }
    });
  }

  onSearch(): void {
    this.currentPage = 1; // Reset to first page when searching
    this.loadStories();
  }

  onPageChange(page: number): void {
    this.currentPage = page;
    this.loadStories();
    
    // Scroll to top when changing pages
    window.scrollTo({ top: 0, behavior: 'smooth' });
  }

  onSearchClear(): void {
    this.searchTerm = '';
    this.onSearch();
  }

  onPageSizeChange(): void {
    this.currentPage = 1; // Reset to first page when changing page size
    this.loadStories();
  }
}
