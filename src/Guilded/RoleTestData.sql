INSERT INTO AspNetRoles(Id, ConcurrencyStamp, Name)
VALUES ('1', '1', 'Admin');
INSERT INTO AspNetRoles(Id, ConcurrencyStamp, Name)
VALUES ('2', '2', 'GM');
INSERT INTO AspNetRoles(Id, ConcurrencyStamp, Name)
VALUES ('3', '3', 'Officer');
INSERT INTO AspNetRoles(Id, ConcurrencyStamp, Name)
VALUES ('4', '4', 'Guest');

INSERT INTO AspNetRoleClaims(ClaimType, ClaimValue, RoleId)
VALUES ('Guilded:Admin:Roles', '1', '1');
INSERT INTO AspNetRoleClaims(ClaimType, ClaimValue, RoleId)
VALUES ('Guilded:Forums:Pin Posts', '1', '1');
INSERT INTO AspNetRoleClaims(ClaimType, ClaimValue, RoleId)
VALUES ('Guilded:Forums:Write Posts', '1', '1');

INSERT INTO AspNetRoleClaims(ClaimType, ClaimValue, RoleId)
VALUES ('Guilded:Admin:Roles', '1', '2');
INSERT INTO AspNetRoleClaims(ClaimType, ClaimValue, RoleId)
VALUES ('Guilded:Forums:Pin Posts', '1', '2');
INSERT INTO AspNetRoleClaims(ClaimType, ClaimValue, RoleId)
VALUES ('Guilded:Forums:Write Posts', '1', '2');

INSERT INTO AspNetRoleClaims(ClaimType, ClaimValue, RoleId)
VALUES ('Guilded:Forums:Pin Posts', '1', '3');
INSERT INTO AspNetRoleClaims(ClaimType, ClaimValue, RoleId)
VALUES ('Guilded:Forums:Write Posts', '1', '3');


INSERT INTO AspNetRoleClaims(ClaimType, ClaimValue, RoleId)
VALUES ('Guilded:Forums:Read Posts', '1', '4');