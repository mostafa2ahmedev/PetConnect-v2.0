import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddProductType } from './add-product-type';

describe('AddProductType', () => {
  let component: AddProductType;
  let fixture: ComponentFixture<AddProductType>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AddProductType]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AddProductType);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
