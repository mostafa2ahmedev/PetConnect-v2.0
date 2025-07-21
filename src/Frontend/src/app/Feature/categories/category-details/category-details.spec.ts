import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CategoryDetails } from './category-details';

describe('CategoryDetails', () => {
  let component: CategoryDetails;
  let fixture: ComponentFixture<CategoryDetails>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CategoryDetails]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CategoryDetails);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
