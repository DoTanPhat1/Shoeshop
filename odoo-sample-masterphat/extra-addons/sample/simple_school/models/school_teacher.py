# from odoo import fields, models, api, _

# class SchoolTeacher(models.Model):
#     _name = 'school.teacher'

#     name = fields.Char()
#     birthday = fields.Date()

# from odoo import fields, models, api, _

# class SchoolStudent(models.Model):
#     _name = 'school.student'

#     name = fields.Char()
#     birthday = fields.Date()

# from odoo import fields, models, api, _

# class SchoolClassroom(models.Model):
#     _name = 'school.classroom'

#     name = fields.Char()

# -*- coding: utf-8 -*-

from odoo import fields, models, api, _


class Teacher(models.Model):
    _name = 'school.teacher'

    name = fields.Char(string='Name')
    birthday = fields.Date(string = 'Birthday')
    # class_ids = fields.Many2many('school.class', string='Classes')

class Student(models.Model):
    _name = 'school.student'

    name = fields.Char(string='Name')
    birthday = fields.Date(string = 'Birthday')
    class_id = fields.Many2one('school.class', string='Class')

class Class(models.Model):
    _name = 'school.class'

    name = fields.Char(string='Name')
    teacher_id = fields.Many2one('school.teacher', string='Teacher')
    student_ids = fields.One2many('school.student', 'class_id', string='Students')
    student_count = fields.Integer(string='Student Count', compute='_compute_student_count')
    
    @api.depends('student_ids')
    def _compute_student_count(self):
        for school in self:
            school.student_count = len(school.student_ids)


# class StudentCount(models.Model):
#     _name = 'school.student.count'

#     count = fields.Integer('Student Count', compute='_compute_student_count')

#     @api.depends('count')
#     def _compute_student_count(self):
#         Student = self.env['school.student']
#         for record in self:
#             record.count = Student.search_count([])
