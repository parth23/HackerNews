import { Component, Input, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PaginatedResult } from '../../models/hacker-news.models';

@Component({
  selector: 'app-pagination',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './pagination.html',
  styleUrls: ['./pagination.scss']
})
export class PaginationComponent {
  @Input() paginatedResult!: PaginatedResult<any>;
  @Output() pageChange = new EventEmitter<number>();
  
  Math = Math; // Make Math available in template

  onPageClick(page: number): void {
    if (page >= 1 && page <= this.paginatedResult.totalPages && page !== this.paginatedResult.pageNumber) {
      this.pageChange.emit(page);
    }
  }

  getPageNumbers(): number[] {
    const current = this.paginatedResult.pageNumber;
    const total = this.paginatedResult.totalPages;
    const delta = 2; // Number of pages to show on each side of current page
    
    const range: number[] = [];
    const rangeWithDots: (number | string)[] = [];
    
    // Always include page 1
    if (total > 0) {
      range.push(1);
    }
    
    // Add pages around current page
    for (let i = Math.max(2, current - delta); i <= Math.min(total - 1, current + delta); i++) {
      range.push(i);
    }
    
    // Always include last page if there are multiple pages
    if (total > 1) {
      range.push(total);
    }
    
    // Remove duplicates and sort
    const uniqueRange = [...new Set(range)].sort((a, b) => a - b);
    
    // Add dots where there are gaps
    for (let i = 0; i < uniqueRange.length; i++) {
      if (i === 0) {
        rangeWithDots.push(uniqueRange[i]);
      } else if (uniqueRange[i] - uniqueRange[i - 1] === 1) {
        rangeWithDots.push(uniqueRange[i]);
      } else {
        rangeWithDots.push('...');
        rangeWithDots.push(uniqueRange[i]);
      }
    }
    
    return uniqueRange;
  }
}
