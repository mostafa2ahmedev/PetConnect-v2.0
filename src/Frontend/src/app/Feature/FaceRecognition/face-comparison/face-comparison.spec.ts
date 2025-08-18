import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FaceComparisonComponent } from './face-comparison';

describe('FaceComparison', () => {
  let component: FaceComparisonComponent;
  let fixture: ComponentFixture<FaceComparisonComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [FaceComparisonComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(FaceComparisonComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
