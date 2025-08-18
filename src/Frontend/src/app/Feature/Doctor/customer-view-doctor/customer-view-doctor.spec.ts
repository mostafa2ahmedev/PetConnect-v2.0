import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CustomerViewDoctor } from './customer-view-doctor';

describe('CustomerViewDoctor', () => {
  let component: CustomerViewDoctor;
  let fixture: ComponentFixture<CustomerViewDoctor>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CustomerViewDoctor]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CustomerViewDoctor);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
