import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AllProductType } from './all-product-type';

describe('AllProductType', () => {
  let component: AllProductType;
  let fixture: ComponentFixture<AllProductType>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AllProductType]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AllProductType);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
