var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews(options =>
{
    // Việt hoá các thông báo lỗi mặc định của model binding (vd: nhập chữ vào ô Điểm)
    var provider = options.ModelBindingMessageProvider;
    provider.SetValueMustNotBeNullAccessor(_ => "Vui lòng nhập giá trị.");
    provider.SetMissingBindRequiredValueAccessor(f => $"Thiếu giá trị cho '{f}'.");
    provider.SetMissingKeyOrValueAccessor(() => "Vui lòng nhập giá trị.");
    provider.SetAttemptedValueIsInvalidAccessor((v, f) => $"Giá trị '{v}' không hợp lệ cho '{f}'.");
    provider.SetUnknownValueIsInvalidAccessor(f => $"Giá trị không hợp lệ cho '{f}'.");
    provider.SetValueIsInvalidAccessor(v => $"Giá trị '{v}' không hợp lệ.");
    provider.SetValueMustBeANumberAccessor(f => $"'{f}' phải là một số thực.");
    provider.SetNonPropertyAttemptedValueIsInvalidAccessor(v => $"Giá trị '{v}' không hợp lệ.");
    provider.SetNonPropertyUnknownValueIsInvalidAccessor(() => "Giá trị không hợp lệ.");
    provider.SetNonPropertyValueMustBeANumberAccessor(() => "Giá trị phải là một số thực.");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
app.UseStaticFiles();