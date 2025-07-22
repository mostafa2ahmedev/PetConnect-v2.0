import { ComponentFixture, TestBed } from '@angular/core/testing';

import { NotFoundDoctor } from './not-found-doctor';

describe('NotFoundDoctor', () => {
  let component: NotFoundDoctor;
  let fixture: ComponentFixture<NotFoundDoctor>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [NotFoundDoctor]
    })
    .compileComponents();

    fixture = TestBed.createComponent(NotFoundDoctor);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
