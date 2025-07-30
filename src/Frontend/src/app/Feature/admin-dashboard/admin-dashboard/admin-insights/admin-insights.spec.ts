import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminInsights } from './admin-insights';

describe('AdminInsights', () => {
  let component: AdminInsights;
  let fixture: ComponentFixture<AdminInsights>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AdminInsights]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AdminInsights);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
