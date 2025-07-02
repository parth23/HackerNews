import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StoryItemComponent } from './story-item';

describe('StoryItemComponent', () => {
  let component: StoryItemComponent;
  let fixture: ComponentFixture<StoryItemComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ StoryItemComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(StoryItemComponent);
    component = fixture.componentInstance;
    component.story = {
      id: 1,
      title: 'Test Story',
      by: 'test author',
      time: 1234567890,
      url: 'http://test.com',
      score: 100,
      descendants: 10,
      type: 'story',
      hasUrl: true,
      timeStamp: new Date()
    };
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
