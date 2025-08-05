import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SellerRegisterForm } from './seller-register-form';

describe('SellerRegisterForm', () => {
  let component: SellerRegisterForm;
  let fixture: ComponentFixture<SellerRegisterForm>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SellerRegisterForm]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SellerRegisterForm);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
