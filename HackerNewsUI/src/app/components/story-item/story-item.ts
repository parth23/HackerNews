import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HackerNewsStory } from '../../models/hacker-news.models';

@Component({
  selector: 'app-story-item',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './story-item.html',
  styleUrls: ['./story-item.scss']
})
export class StoryItemComponent {
  @Input() story!: HackerNewsStory;
  @Input() index!: number;

  getTimeAgo(timestamp: Date): string {
    const now = new Date();
    const diffInMinutes = Math.floor((now.getTime() - timestamp.getTime()) / (1000 * 60));
    
    if (diffInMinutes < 60) {
      return `${diffInMinutes} minutes ago`;
    }
    
    const diffInHours = Math.floor(diffInMinutes / 60);
    if (diffInHours < 24) {
      return `${diffInHours} hours ago`;
    }
    
    const diffInDays = Math.floor(diffInHours / 24);
    return `${diffInDays} days ago`;
  }

  getDomainFromUrl(url: string): string {
    try {
      const domain = new URL(url).hostname;
      return domain.replace('www.', '');
    } catch {
      return '';
    }
  }
}
