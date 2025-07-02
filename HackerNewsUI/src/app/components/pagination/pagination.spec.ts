import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PaginationComponent } from './pagination';

describe('PaginationComponent', () => {
  let component: PaginationComponent;
  let fixture: ComponentFixture<PaginationComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PaginationComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PaginationComponent);
    component = fixture.componentInstance;
    component.paginatedResult = {
      items: [],
      pageNumber: 1,
      pageSize: 10,
      totalPages: 1,
      totalCount: 10,
      hasNext: true,
      hasPrevious: false
    };
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
