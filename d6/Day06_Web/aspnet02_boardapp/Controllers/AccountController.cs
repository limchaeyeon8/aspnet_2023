using aspnet02_boardapp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace aspnet02_boardapp.Controllers
{
    // 사용자 회원가입 / 로그인 / 로그아웃
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            // 생성자 마법사로 만드세요
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _signInManager = signInManager ?? throw new ArgumentNullException(nameof(_signInManager));
        }

        // 
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        // public IActionResult Register(egisterModel model)
        // 비동기가 아니면 return값은 IActionResult
        // 비동기가 되면 Task<IActionResult>
        public async Task <IActionResult> Register(RegisterModel model)
        {
            ModelState.Remove("PhoneNumber");   // PhoneNumber는 입력값 검증에서 제거

            if (ModelState.IsValid)         // 데이터를 제대로 입력해서 검증 성공하면 
            {
                // ASP.NET user - aspnetusers 테이블에 데이터 넣기 위해서
                // 매핑(mapping)되는 인스턴스 생성
                var user = new IdentityUser()
                {
                    UserName = model.Email,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber // 폰번호 추가
                };
                // aspnetusers 테이블에 사용자 데이터를 대입
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    // 회원가입 성공했으면 로그인 -> localhost:7125/Home/Index 로 가라
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    // ---- TODO: 회원가입 후 토스트 메시지 띄우기
                    TempData["succeed"] = "회원이된 걸 환영합니다";  // 성공 메세지

                    return RedirectToAction("Index", "Home");   // 첫번째 화면   
                }

                // 오류난다면
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);     // 회원가입을 실패하면 그 화면 그대로 유지
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                // 파라미터 순서 : ID / PW / isPersistent = RememverMe, 고그인실패 시 계정 잠금
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);    // 오류났을 때 계정을 잠그겠냐=false

                if (result.Succeeded)
                {
                    // ---- TODO: 로그인 후 토스트 메시지 띄우기
                    TempData["succeed"] = "로그인됐습니다";  // 성공 메세지

                    return RedirectToAction("Index", "Home");

                }

                ModelState.AddModelError("", "로그인 실패");     // 입력검증이 실패하면 화면에 그대로 대기
            }
            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            // ---- TODO: 로그아웃 후 토스트 메시지 띄우기
            TempData["succeed"] = "로그아웃됐습니다";  // 성공 메세지

            return RedirectToAction("Index", "Home");
        }
    }
}
