do $$
begin
for i in 1..10 loop

insert into test122 (text,value)
values (i+1, 123+i);
end loop;
end;$$;