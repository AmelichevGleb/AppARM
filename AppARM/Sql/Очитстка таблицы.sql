do $$
begin
for i in 1..1000 loop

delete from test122 
where id = i;
end loop;
end;$$;