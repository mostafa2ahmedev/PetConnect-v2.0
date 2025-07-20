import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CustomerRegisterForm } from './customer-register-form';

describe('CustomerRegisterForm', () => {
  let component: CustomerRegisterForm;
  let fixture: ComponentFixture<CustomerRegisterForm>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CustomerRegisterForm]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CustomerRegisterForm);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
