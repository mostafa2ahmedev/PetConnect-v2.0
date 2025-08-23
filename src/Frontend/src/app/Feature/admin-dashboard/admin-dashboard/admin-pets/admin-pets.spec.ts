import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminPets } from './admin-pets';

describe('AdminPets', () => {
  let component: AdminPets;
  let fixture: ComponentFixture<AdminPets>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AdminPets]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AdminPets);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
