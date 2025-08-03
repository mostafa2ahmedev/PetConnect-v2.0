import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SellerDashboard } from './seller-dashboard';

describe('SellerDashboard', () => {
  let component: SellerDashboard;
  let fixture: ComponentFixture<SellerDashboard>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SellerDashboard]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SellerDashboard);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
