import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ReviewCustomerListOfReviews } from './review-customer-list-of-reviews';

describe('ReviewCustomerListOfReviews', () => {
  let component: ReviewCustomerListOfReviews;
  let fixture: ComponentFixture<ReviewCustomerListOfReviews>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ReviewCustomerListOfReviews]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ReviewCustomerListOfReviews);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
