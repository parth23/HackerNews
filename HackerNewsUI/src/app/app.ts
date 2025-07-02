import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { StoriesComponent } from './components/stories/stories';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, StoriesComponent],
  templateUrl: './app.html',
  styleUrls: ['./app.scss']
})
export class App {
  protected title = 'HackerNewsUI';
}
