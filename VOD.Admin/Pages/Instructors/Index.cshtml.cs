using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VOD.Admin.Models;
using VOD.Domain.Interfaces;

using VOD.Domain.DTOModles.Admin;
using VOD.Domain.Entities;
using System.Linq.Expressions;
using System.Reflection;

namespace VOD.Admin.Pages.Instructors
{
    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        public IEnumerable<InstructorDTO> Items { get; set; } = new List<InstructorDTO>();
        [TempData]
        public string Alert { get; set; }

        private readonly IAdminService _adminService;
        public IndexModel(IAdminService adminService)
        {
            _adminService = adminService;
        }
        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                //  Expression<Func<Instructor, bool>> equalExpr = instr => instr.Id.Equals(3);
                // var lambda = equalExpr as LambdaExpression;

                //GetProperties<Instructor>(instr => instr.Id == 3);
                GetProperties<Instructor>(instr => instr.Id.Equals(3));

                //GetExpressionProperties(lambda);

                Items = await _adminService.GetAsync<Instructor, InstructorDTO>(true);
                return Page();
            }
            catch (Exception exp)
            {
                Alert = "You do not have access to this page.";
                return RedirectToPage("/Index");
            }

        }
        private Dictionary<string, object> _properties = new Dictionary<string, object>();
        private void GetExpressionProperties(Expression expression)
        {

            var body = (MethodCallExpression)expression;
            var argument = body.Arguments[0];
            if (argument is MemberExpression)
            {
                var memberExpression = argument as MemberExpression;
                var value = ((FieldInfo)memberExpression.Member).GetValue(
                    ((ConstantExpression)memberExpression.Expression).Value);

                _properties.Add(memberExpression.Member.Name, value);
            }

        }

        private void ResolveExpression(Expression expression)
        {
            try
            {
                if (expression is BinaryExpression)
                {
                    if (expression.NodeType == ExpressionType.AndAlso)
                    {
                        var binaryExpression = expression as BinaryExpression;
                        ResolveExpression(binaryExpression.Left);
                        ResolveExpression(binaryExpression.Right);
                    }
                    else if (expression.NodeType == ExpressionType.Equal)
                    {
                        var binaryExpression = expression as BinaryExpression;
                        ResolveExpression(binaryExpression.Left);
                        ResolveExpression(binaryExpression.Right);
                    }
                }
                else if (expression is ConstantExpression)
                {
                    var value = ((ConstantExpression)expression).Value;
                    var tmp = _properties.GetValueOrDefault("temp");
                    if (tmp != null)
                    {
                        _properties.Add(tmp.ToString(), value);
                        _properties.Remove("temp");
                    }
                    else
                    {
                        _properties.Add("temp", value);
                    }
                }
                else if (expression is MemberExpression)
                {
                    var memberExpression = expression as MemberExpression;
                    var tmp = _properties.GetValueOrDefault("temp");
                    if (tmp != null)
                    {
                        _properties.Add(memberExpression.Member.Name, tmp);
                        _properties.Remove("temp");
                    }
                    else
                    {
                        _properties.Add("temp", memberExpression.Member.Name);
                    }

                }
                else if (expression is MethodCallExpression)
                {
                    var body = expression as MethodCallExpression;
                    if (body.Object is MemberExpression)
                    {
                        var arg = body.Arguments[0];
                        var memberExpression = body.Object as MemberExpression;
                        var val = ((ConstantExpression)arg).Value;

                        _properties.Add(memberExpression.Member.Name, val);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void GetProperties<TSource>(Expression<Func<TSource, bool>> expression)
        {
            try
            {
                var lambda = expression as LambdaExpression;
                _properties.Clear();
                ResolveExpression(lambda.Body);
                var typeName = typeof(TSource).Name;

                if (!typeName.Equals("Instructor") && !typeName.Equals("Course") && !_properties.ContainsKey("courseId"))
                    _properties.Add("courseId", 0);
            }
            catch { throw; }
        }
    }
}