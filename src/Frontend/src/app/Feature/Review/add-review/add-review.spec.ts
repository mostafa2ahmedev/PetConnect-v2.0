import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddReview } from './add-review';

describe('AddReview', () => {
  let component: AddReview;
  let fixture: ComponentFixture<AddReview>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AddReview]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AddReview);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
