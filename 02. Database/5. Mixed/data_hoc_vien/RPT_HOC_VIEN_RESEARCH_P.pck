create or replace package RPT_HOC_VIEN_RESEARCH_P is

  -- Author  : ADMIN
  -- Created : 14/06/2011 1:45:52 PM
  -- Purpose : 
  
 /*
|| fill dataset
*/
procedure filldataset(op_ref_cur out DB_UTILS_P.ODPNET_REFCURSOR,ip_reporting_user varchar2);
/*function get ky nop tien cuoi cung*/
function get_ky_cuoi_cung_nop_hoc_phi(ip_id_hoc_vien in number) return number;
/*function get diem_trung_binh_tich_luy*/
function get_diem_trung_binh_tich_luy(ip_id_hoc_vien in number) return number;
/*
procedure ho_so_hoc_vien
*/
procedure fill_ho_so_hoc_vien(op_ref_cur out DB_UTILS_P.ODPNET_REFCURSOR);
/*
procedure fill hoc vien theo lop
*/
procedure fill_hoc_vien_theo_lop(op_ref_cur out DB_UTILS_P.ODPNET_REFCURSOR);
/*
procedure fill diem_tb_tich_luy_ky
*/
procedure fill_diem_tb_tich_luy_ky(op_ref_cur out DB_UTILS_P.ODPNET_REFCURSOR);
end RPT_HOC_VIEN_RESEARCH_P;
/
create or replace package body RPT_HOC_VIEN_RESEARCH_P is
/*
|| fill dataset
*/
procedure filldataset(op_ref_cur out DB_UTILS_P.ODPNET_REFCURSOR,ip_reporting_user varchar2) is
begin
/*
              -- delete
              delete from rpt_hoc_vien_research where reporting_user = ip_reporting_user;
              -- insert
              insert into rpt_hoc_vien_research(id_hoc_vien,reporting_user)
              select t.id,ip_reporting_user from dm_hoc_vien t
                where t.trang_thai in (--constants_p.C_TRANG_THAI_HV_TRUNG_TUYEN
                                    constants_p.C_TRANG_THAI_HV_BL_KQ_SO_TUYEN
                                    ,constants_p.C_TRANG_THAI_HV_KY_CK_NOP_HS
                                    ,constants_p.C_TRANG_THAI_HV_DA_CO_QD
                                    ,constants_p.C_TRANG_THAI_HV_BL_KQ_HOC_TAP
                                    ,constants_p.C_TRANG_THAI_HV_N_HOC_CO_DON
                                    ,constants_p.C_TRANG_THAI_HV_TAM_NGUNG_HOC);
                                    */
              
              -- update
              for v_rec in (select t.id_hoc_vien from rpt_hoc_vien_research t where t.reporting_user = ip_reporting_user
              and t.id_hoc_vien in (select distinct id_hoc_vien from dm_hoc_vien_theo_hoc))
              loop
                  update rpt_hoc_vien_research k
                         set --k.y2_ky_hoc_cuoi_cung_nop_hp = get_ky_cuoi_cung_nop_hoc_phi(v_rec.id_hoc_vien)
                             --,
                             k.Y4_DIEM_TRUNG_BINH_TICH_LUY = get_diem_trung_binh_tich_luy(V_rec.Id_Hoc_Vien)
                  where k.id_hoc_vien = v_rec.id_hoc_vien;
              end loop;
              
              -- fill                     
              open op_ref_cur for 
              select 
                   to_number(substr(to_char(sysdate,'dd/mm/yyyy'),7,4))
                   - to_number(substr(to_char(t.ngay_thang_nam_sinh,'dd/mm/yyyy'),7,4)) as x1_tuoi_hoc_vien
                   ,case 
                         when t.gioi_tinh_yn = 'Y' then 1
                         when t.gioi_tinh_yn = 'N' then 0
                    end as gioi_tinh_yn
                   ,t.xep_loai_tot_nghiep as x3_bang_tot_nghiep_cap_3_loai
                   ,0 as x7_la_dan_toc_thieu_so
                   ,t.ton_giao as x8_ton_giao
                   ,t.ma_doi_tuong_tuyen_sinh as x14_doi_tuong_hoc
                   ,t.nganh_tuyen_sinh as x15_nganh_hoc
                   ,k.y2_ky_hoc_cuoi_cung_nop_hp
                   ,t.trang_thai as y3_trang_thai_hoc_vien
                   ,t.noi_sinh
                   ,t.diem1
                   ,t.diem2
                   ,t.diem3
                   ,t.khac_nganh_yn
                   ,t.ma_khoi_nganh_da_tot_nghiep
                   ,t.da_co_quyet_dinh_yn
                   ,t.da_tot_nghiep_yn
                   ,t.ngay_thang_nam_sinh   
                   ,t.dan_toc_cha_yn
                   ,t.ton_giao_cha
                   ,t.dan_toc_me_yn
                   ,t.ton_giao_me
                   ,t.dan_toc_vo_chong_yn
                   ,t.ton_giao_vo_chong
                   ,k.y4_diem_trung_binh_tich_luy
                from dm_hoc_vien t,rpt_hoc_vien_research k
                where t.id = k.id_hoc_vien and k.reporting_user = ip_reporting_user;                                    
end filldataset;
/*function get ky nop tien cuoi cung*/
function get_ky_cuoi_cung_nop_hoc_phi(ip_id_hoc_vien in number) return number
is
v_num_max_stt_dot number :=0;
begin
    for v_rec in (select 1 from phieu_thu_chi p
                          ,dm_dot_hoc d
                   where p.id_dot_hoc = d.id
                   and p.id_muc_dich_thu_chi = 15476 and p.id_hoc_vien = ip_id_hoc_vien) 
    loop
        select max(d.dot_hoc_so) into v_num_max_stt_dot from phieu_thu_chi p
                          ,dm_dot_hoc d
                   where p.id_dot_hoc = d.id
                   and p.id_muc_dich_thu_chi = 15476
                   and p.id_hoc_vien = ip_id_hoc_vien;
    end loop;
    return v_num_max_stt_dot;
end get_ky_cuoi_cung_nop_hoc_phi;
/*function get diem_trung_binh_tich_luy*/
function get_diem_trung_binh_tich_luy(ip_id_hoc_vien in number) return number
is
v_num_diem number(5,2) :=0;
begin
   
    select sum(t.diem_tong_ket_mon)/count(t.id) into v_num_diem from dm_hoc_vien_theo_hoc t
    where t.id_hoc_vien = ip_id_hoc_vien;
    return v_num_diem;
end get_diem_trung_binh_tich_luy;
/*
procedure ho_so_hoc_vien
*/
procedure fill_ho_so_hoc_vien(op_ref_cur out DB_UTILS_P.ODPNET_REFCURSOR) is
begin
     open op_ref_cur for
     select t.ma_hoc_vien
            ,to_number(substr(to_char(sysdate,'dd/mm/yyyy'),7,4))
                   - to_number(substr(to_char(t.ngay_thang_nam_sinh,'dd/mm/yyyy'),7,4)) as tuoi
            ,case 
                  when t.gioi_tinh_yn = 'Y' then 'NAM'
                  when t.gioi_tinh_yn = 'N' then 'NU'
             end as gioi_tinh
            ,t.ma_doi_tuong_tuyen_sinh
            ,t.khac_nganh_yn
            ,t.ma_khoi_nganh_da_tot_nghiep
            ,t.global_username
     from dm_hoc_vien t
     where t.global_username is not null
     and t.trang_thai in (constants_p.C_TRANG_THAI_HV_TRUNG_TUYEN
                          ,constants_p.C_TRANG_THAI_HV_BL_KQ_SO_TUYEN
                          ,constants_p.C_TRANG_THAI_HV_KY_CK_NOP_HS
                          ,constants_p.C_TRANG_THAI_HV_DA_CO_QD
                          ,constants_p.C_TRANG_THAI_HV_BL_KQ_HOC_TAP
                          ,constants_p.C_TRANG_THAI_HV_N_HOC_CO_DON
                          ,constants_p.C_TRANG_THAI_HV_TAM_NGUNG_HOC);
end fill_ho_so_hoc_vien;
/*
procedure fill hoc vien theo lop
*/
procedure fill_hoc_vien_theo_lop(op_ref_cur out DB_UTILS_P.ODPNET_REFCURSOR)
is
begin
     open op_ref_cur for
     select 
            t.ma_hoc_vien
            ,l.ma_lop
     from dm_hoc_vien t, dm_lop_quan_ly l
     where t.id_lop_quan_ly = l.id
     and t.global_username is not null
     and t.trang_thai in (constants_p.C_TRANG_THAI_HV_TRUNG_TUYEN
                          ,constants_p.C_TRANG_THAI_HV_BL_KQ_SO_TUYEN
                          ,constants_p.C_TRANG_THAI_HV_KY_CK_NOP_HS
                          ,constants_p.C_TRANG_THAI_HV_DA_CO_QD
                          ,constants_p.C_TRANG_THAI_HV_BL_KQ_HOC_TAP
                          ,constants_p.C_TRANG_THAI_HV_N_HOC_CO_DON
                          ,constants_p.C_TRANG_THAI_HV_TAM_NGUNG_HOC);
end fill_hoc_vien_theo_lop;

/*
procedure fill diem_tb_tich_luy_ky
*/
procedure fill_diem_tb_tich_luy_ky(op_ref_cur out DB_UTILS_P.ODPNET_REFCURSOR)
is
begin
     open op_ref_cur for
        select t.ma_hoc_vien, l.dot_to_chuc_dao_tao as ky 
             ,sum(k.diem_tong_ket_mon)/count(t.ma_hoc_vien) as diem_tb_tich_luy
              from dm_hoc_vien_theo_hoc k, dm_hoc_vien t,dm_lop_hoc l
        where k.id_hoc_vien = t.id
         and t.global_username is not null
         and t.trang_thai in (constants_p.C_TRANG_THAI_HV_TRUNG_TUYEN
                      ,constants_p.C_TRANG_THAI_HV_BL_KQ_SO_TUYEN
                      ,constants_p.C_TRANG_THAI_HV_KY_CK_NOP_HS
                      ,constants_p.C_TRANG_THAI_HV_DA_CO_QD
                      ,constants_p.C_TRANG_THAI_HV_BL_KQ_HOC_TAP
                      ,constants_p.C_TRANG_THAI_HV_N_HOC_CO_DON
                      ,constants_p.C_TRANG_THAI_HV_TAM_NGUNG_HOC)
        and k.id_lop_hoc = l.id
        group by t.ma_hoc_vien
            ,l.dot_to_chuc_dao_tao;
end fill_diem_tb_tich_luy_ky;      
end RPT_HOC_VIEN_RESEARCH_P;
/
