# 💼 CareerBuilderX

> An AI-powered Resume and Portfolio Builder built with ASP.NET Core MVC and OpenAI. Designed to help users create professional CVs and portfolios with ease, automation, and style.

---

## 🚀 Features

- ✨ **AI Resume Parser**: Uses OpenAI to extract structured data from raw input
- 🎨 **Beautiful Resume Templates**: Print-ready and modern designs
- 💡 **Portfolio Builder**: Showcase services, projects, and skills dynamically
- 📬 **SMTP Email Support**: For account verification, notifications, etc.
- 🔐 **ASP.NET Core Identity Integration**: User authentication and role-based access
- 🌐 **Responsive UI** using Bootstrap 5 and Razor Views
- 🛠️ **Modular Design** with Repositories, DTOs, and ViewModels

---

## 🖥️ Technologies Used

| Layer         | Tech Stack                              |
|---------------|------------------------------------------|
| Frontend      | HTML, CSS, Bootstrap 5, Razor Views     |
| Backend       | ASP.NET Core MVC (.NET 8)               |
| AI Service    | OpenAI (GPT-4 via Semantic Kernel)      |
| Database      | SQL Server                              |
| Identity/Auth | ASP.NET Core Identity                   |
| Deployment    | Azure App Service + GitHub              |

---

## 📸 Screenshots (Coming Soon)

> Add screenshots or GIFs of:
> - Resume generation page
> - Portfolio layout
> - AI-generated summary

---

## 📦 Project Structure

```bash
CareerBuilderX/
├── Controllers/
├── Views/
├── Models/
├── Services/
├── Documents/           # PDF generation logic
├── wwwroot/
├── appsettings.json
└── README.md
```

---

## ⚙️ How to Run Locally

1. Clone the repository:
   ```bash
   git clone https://github.com/Omar-Sarhan/CareerBuilderX.git
   ```

2. Restore NuGet packages:
   ```bash
   dotnet restore
   ```

3. Update your secrets (`appsettings.Development.json`) with:
   - SQL connection string
   - OpenAI API Key
   - SMTP credentials

4. Run the app:
   ```bash
   dotnet run
   ```

---

## 📬 Contact

- Developer: **Omar Sarhan**
- GitHub: [@Omar-Sarhan](https://github.com/Omar-Sarhan)
- Email: [📧 Send Email](mailto:smalljohn148@gmail.com)

---

## 📄 License

MIT License. You can use, modify, and distribute this project freely.

> Built with ❤️ and .NET by Omar Sarhan