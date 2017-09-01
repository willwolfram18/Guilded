INSERT INTO AspNetRoles(Id, ConcurrencyStamp, Name, NormalizedName)
VALUES ('1', '1', 'Admin', 'ADMIN');
INSERT INTO AspNetRoles(Id, ConcurrencyStamp, Name, NormalizedName)
VALUES ('2', '2', 'Guest', 'GUEST');

INSERT INTO AspNetRoleClaims(ClaimType, ClaimValue, RoleId)
VALUES ('https://examples.org/claims/permission', 'guilded.admin.roles', '1');