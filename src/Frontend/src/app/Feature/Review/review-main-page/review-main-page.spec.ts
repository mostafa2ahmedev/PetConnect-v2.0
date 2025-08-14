import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ReviewMainPage } from './review-main-page';

describe('ReviewMainPage', () => {
  let component: ReviewMainPage;
  let fixture: ComponentFixture<ReviewMainPage>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ReviewMainPage]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ReviewMainPage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
