FOR /L %%N IN (2,1,20000) DO (
  hey_windows_amd64.exe -n 1 -c 1 -m POST -d {"userName":"johndoe_%%N","firstName":"John","lastName":"Doe","email":"bestjohn@doe%%N.com","phone":"+71002003040"} "http://arch.homework/app/api/v1/users"
  hey_windows_amd64.exe -n 10 -c 10 -m DELETE "http://arch.homework/app/api/v1/users/%%N"
)
