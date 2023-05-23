from odoo import fields, models, api, _


class Schoolbadge(models.Model):
    _name = 'school.badge'

    name = fields.Char('Ten')

class SchoolTeacher(models.Model):
    _inherit = 'school.teacher'

    huyhieu_ids = fields.Many2many('school.badge')



    

# class Class(models.Model):
#     _inherit = 'school.class'

#     student_ids = fields.One2many('school.student', 'class_id', string='Students')
#     teacher_ids = fields.Many2one('school.teacher', string='Teacher')
#     student_ids = fields.One2many('school.student', 'class_id', string='Students')
