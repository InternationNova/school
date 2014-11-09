using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using school.Models;
using school.Classes;
using System.Data.SqlClient;
using System.Configuration;

namespace school.Controllers
{
    public class smkItmesController : Controller
    {
        private GlobalInfo db = new GlobalInfo();

        string str_connection = ConfigurationManager.ConnectionStrings["GlobalInfo"].ConnectionString;
        //
        // GET: /smkItmes/

        public ActionResult Index()
        {
            if (Session["USERNAME"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View(db.smk_itens.ToList());
        }

        //
        // GET: /smkItmes/Details/5
        public ActionResult buscaItem(string cod_smk ,string op) 
        {

            List<materia_primas> materia_primaArr= new List<materia_primas>();
            switch (op) 
            
            {
                case "item_smk":
                    string conteudo1 = "0" ;  
                    materia_primaArr = getMateriaPrimas(cod_smk) ;
                    if (materia_primaArr.Count > 0)
                    {
                        for(var i = 0 ; i < materia_primaArr.Count; i++)
                        {
                            conteudo1 = materia_primaArr[i].descricao + "<input type='hidden' name='materia_primas_id' id='materia_primas_id' value='" + materia_primaArr[i].id + "' />";
                        }

                        return Json(new { conteudo = conteudo1 });
                    }
                    else {

                        return Json(new { conteudo = "0" });
                    }
                   
                    break;
                case "verificaCadastro":
                        return Json(new { conteudo = "0" });        
                    break;
                default:
                        return Json(new { conteudo = "Falta parâmetros" });
                    break;
            
            
            }
            
            if (Session["USERNAME"] == null)
            {
                return Json(new { ok = "failed" });

            }
            else
            {
                return Json(new { ok = "success" });
            }

        
         }

        public ActionResult calculaGasto(string cod, string codSubProduto , string op,string s , string i , string d, string e , string f ,string v) 
        {

            sub_produtos subProdutosObj = new sub_produtos();
            subProdutosObj = db.sub_produtos.Find(Convert.ToInt32(codSubProduto));
            decimal total = 0;
            string opString = op;
            double quantidadeLargura = 0;
            double quantidadeComprimento = 0;
            double quantidadeArea = 0;
            int codInt = Convert.ToInt32(cod);

            if ((opString == "madeira") || (opString == "mdf"))
            {
                 total = subProdutosObj.area * subProdutosObj.perda / 100 + subProdutosObj.area;
                
            }else if(opString == "fita_borda")
		    {
                List<perda> perdaArr= new List<perda>();


                
                perdaArr = (from x in db.perdas
                            where x.materia_primas_id == codInt
                     select x
                   ).ToList();
                for(var j = 0; j < perdaArr.Count ; j++)
                {
                    if((Convert.ToInt32(s) == 1) && ( Convert.ToInt32(i) == 1)){
                        quantidadeLargura = subProdutosObj.largura_acabada*2 + perdaArr[j].quantidade*2;

                    }
                    else if ((Convert.ToInt32(s) == 1) || (Convert.ToInt32(i) == 1))
                    {
                        
                        quantidadeLargura = subProdutosObj.largura_acabada + perdaArr[j].quantidade ;
                    }
                    if(( Convert.ToInt32(d) == 1) && ( Convert.ToInt32(e) == 1))
                    {
                        quantidadeComprimento = subProdutosObj.comprimento_acabada*2 + perdaArr[j].quantidade*2;
                    }
                    else if ((Convert.ToInt32(d) == 1) || (Convert.ToInt32(e) == 1))
                    {
                        quantidadeComprimento = subProdutosObj.comprimento_acabada  + perdaArr[j].quantidade;
                    }
                 }
                    total = Convert.ToDecimal(((quantidadeComprimento + quantidadeLargura)* subProdutosObj.quantidade)/1000);

            }else if(opString == "polistein")
            {
                   
                    List<consumos> consumosArr = new List<consumos>();
                    
                     consumosArr = (from x in db.consumos
                                    where x.materia_primas_id == codInt
                                    select x
                                    ).ToList();
                    for (var j = 0; j < consumosArr.Count ; j++)
                    {
                        if(( Convert.ToInt32(s) == 1) && ( Convert.ToInt32(i) == 1))
                        {
                            quantidadeLargura = ((((double)(subProdutosObj.largura_acabada) / 1000.0) * ((double)(subProdutosObj.espessura_acabada) / 1000)) * 2) * (consumosArr[j].quantidade * 2);
                        }
                        else if ((Convert.ToInt32(s) == 1) || (Convert.ToInt32(i) == 1))
                        {
                            quantidadeLargura = (((double)(subProdutosObj.largura_acabada)/1000.0)*((double)(subProdutosObj.espessura_acabada)/1000.0))*consumosArr[j].quantidade;
                        }
                        if((Convert.ToInt32(d) == 1) && (Convert.ToInt32(e) == 1))
                        {
                        
                            quantidadeComprimento = (((double)(subProdutosObj.comprimento_acabada)/1000.0)*((double)(subProdutosObj.espessura_acabada)/1000.0)*2)*(consumosArr[j].quantidade*2);
                        }
                        else if ((Convert.ToInt32(d) == 1) || (Convert.ToInt32(e) == 1))
                        {
                            quantidadeComprimento = (((double)(subProdutosObj.comprimento_acabada) / 1000.0) * ((double)(subProdutosObj.espessura_acabada) / 1000.0)) * (consumosArr[j].quantidade * 2);
                        }
                        if((Convert.ToInt32(f) == 1) && (Convert.ToInt32(v) == 1))
                        {
                            quantidadeArea = ((((double)(subProdutosObj.largura_acabada) / 1000.0) * ((double)(subProdutosObj.espessura_acabada) / 1000.0)) * 2) * (consumosArr[j].quantidade * 2);
                        }
                        else if ((Convert.ToInt32(f) == 1) || (Convert.ToInt32(v) == 1))
                        {
                            quantidadeArea = (((double)(subProdutosObj.comprimento_acabada) / 1000.0) * ((double)(subProdutosObj.espessura_acabada) / 1000.0)) * (consumosArr[j].quantidade * 2);
                        }                 
                    }
                    total = Convert.ToDecimal(((quantidadeComprimento + quantidadeLargura + quantidadeArea )*subProdutosObj.quantidade)); 
			       
            }else if(opString == "cola_hotmelt")
            {
                List<sub_produtos_materia_primas> sub_produtos_materia_primasArr = new List<sub_produtos_materia_primas>();

                sub_produtos_materia_primasArr = (from x in db.sub_produtos_materia_primas
                                        where x.sub_produtos_id == subProdutosObj.id &&
                                              x.calculo == "fita_borda"
                                         select x
                                            ).ToList();
                for(var j = 0 ; j < sub_produtos_materia_primasArr.Count;j++){
                    List<consumos> consumosArr = new List<consumos>();

                    consumosArr = (from x in db.consumos
                                   where x.materia_primas_id == codInt
                                   select x).ToList();

                    for( var k = 0 ; k < consumosArr.Count;k++){
                        total = Convert.ToDecimal((subProdutosObj.quantidade * ((double)(subProdutosObj.espessura_acabada) / 1000.0)) * consumosArr[k].quantidade);
                    }

                }
                
                                    
            }else if(opString == "cola_contato"){
                
                List<consumos> consumosArr = new List<consumos>();

                    consumosArr = (from x in db.consumos
                                   where x.materia_primas_id == codInt
                                   select x).ToList();
                for(var j = 0 ; j < consumosArr.Count;j++){
                    
                    total = Convert.ToDecimal((double)(subProdutosObj.area) * consumosArr[j].quantidade);
                    
                }
                
            }
            else if(opString == "laminado")
		    {
				//area * perda cadastrada no subproduto
				//$total = $dadosSubProduto['area'] * ($dadosQuantidadePerda['quantidade']);
                total = Convert.ToDecimal((double)(subProdutosObj.area) * (double)(subProdutosObj.area) / 1000.0 + (double)(subProdutosObj.area));
		    }
			    
		    
            else {

                total = -1;
            }


            return Json(new { result = total });
        }
        [HttpPost]
        public ActionResult registerSubProdutos(string codSubProduto, string idSmkItem, string materia_primas_id, string quantidade, string calculo, string superior, string inferior, string esquerda, string direita)
        {

            string str_query = @"INSERT INTO sub_produtos_materia_primas 
                                   (sub_produtos_id,materia_primas_id,quantidade,calculo ,
                                    superior,inferior,esquerda,direita)
                                  VALUES (@sub_produtos_id,@materia_primas_id,@quantidade,@calculo,
                                          @superior ,@inferior ,@esquerda , @direita

                                    )";

            using (SqlConnection conn = new SqlConnection(str_connection))
            using (SqlCommand cmd = new SqlCommand(str_query, conn))
            {

                cmd.Parameters.AddWithValue("@sub_produtos_id", Convert.ToInt32(codSubProduto));
                cmd.Parameters.AddWithValue("@materia_primas_id", Convert.ToInt32(materia_primas_id));
                cmd.Parameters.AddWithValue("@quantidade", Convert.ToDouble(quantidade));
                cmd.Parameters.AddWithValue("@calculo",calculo );
                cmd.Parameters.AddWithValue("@superior", Convert.ToInt32(superior));
                cmd.Parameters.AddWithValue("@inferior", Convert.ToInt32(inferior));
                cmd.Parameters.AddWithValue("@esquerda", Convert.ToInt32(esquerda));
                cmd.Parameters.AddWithValue("@direita", Convert.ToInt32(direita));
               
                 conn.Open();


                cmd.ExecuteNonQuery();
                conn.Close();
                conn.Dispose();


                conn.Close();
             }

            return  RedirectToAction("Details", new { id = Convert.ToInt32(idSmkItem) });                                                              
             
        
        }

        public List<materia_primas> getMateriaPrimas(string cod_smk) 
        {   
            List<materia_primas> materia_primaArr= new List<materia_primas>();
           string str_query = @"select * from materia_primas where codigo_smk = @cod_smk";

            using (SqlConnection conn = new SqlConnection(str_connection))
            using (SqlCommand cmd = new SqlCommand(str_query, conn))
            {
                cmd.Parameters.AddWithValue("@cod_smk", cod_smk);

                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    materia_primas materia_primasObj = new materia_primas();
                    int materiaPrimaId = Convert.ToInt32(reader["id"]);
                    materia_primasObj.id = materiaPrimaId;
                    materia_primasObj.descricao = reader["descricao"].ToString(); 
                    materia_primaArr.Add(materia_primasObj);

                }

            }

            return materia_primaArr;
        }

        public ViewResult Details(int id)
        {

            smkProductView smkProductView = new smkProductView();
            List<smkSubProdus> smkSubProdus = new List<smkSubProdus>();


           
           

            List<smkAccessory> smkSubAccessory = new List<smkAccessory>();
            List<smkAccessory> smkAccessory1 = new List<smkAccessory>();
            List<smkAccessory> smkAccessory2 = new List<smkAccessory>();


            smkProductView.id = id;
            string str_query = @"SELECT sp.id, sp.descricao, comprimento_acabada, largura_acabada, espessura_acabada, comprimento_bruto, 
								largura_bruto, espessura_bruto, quantidade, AREA, perda, 
									smk_itens_id, csp.descricao AS csp_descricao FROM sub_produtos sp 
									JOIN categoria_sub_produtos csp ON csp.id = sp.categoria_sub_produtos_id
									WHERE sp.smk_itens_id =@id ORDER BY csp.descricao";

            using (SqlConnection conn = new SqlConnection(str_connection))
            using (SqlCommand cmd = new SqlCommand(str_query, conn))
            {
                cmd.Parameters.AddWithValue("@id", id);

                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    int nId = Convert.ToInt32(reader["id"]);
                    string descricao = reader["descricao"].ToString();
                    string comprimento_acabada = reader["comprimento_acabada"].ToString();
                    string largura_acabada = reader["largura_acabada"].ToString();
                    string espessura_acabada = reader["espessura_acabada"].ToString();
                    string comprimento_bruto = reader["comprimento_bruto"].ToString();
                    string largura_bruto = reader["largura_bruto"].ToString();
                    string espessura_bruto = reader["espessura_bruto"].ToString();
                    double quantidade = Convert.ToDouble(reader["quantidade"]);
                    string area = reader["Area"].ToString();
                    string perda = reader["perda"].ToString();
                    int smkItemId = Convert.ToInt32(reader["smk_itens_id"]);
                    string csp_descricao = reader["csp_descricao"].ToString();

                    smkSubProdus objsubProdus = new smkSubProdus();


                    objsubProdus.id = nId;
                    objsubProdus.sp_descricao = descricao;
                    objsubProdus.comprimento_acabada = comprimento_acabada;
                    objsubProdus.espessura_acabada = espessura_acabada;
                    objsubProdus.comprimento_bruto = comprimento_bruto;
                    objsubProdus.largura_bruto = largura_bruto;
                    objsubProdus.quantidade = quantidade;
                    objsubProdus.area = area;
                    objsubProdus.perda = perda;
                    objsubProdus.smk_itens_id = smkItemId;
                    objsubProdus.csp_descricao = csp_descricao;
                    objsubProdus.largura_acabada = largura_acabada;

                    smkSubAccessory = getSmkProduct(nId);
                    objsubProdus.smkAccessory = smkSubAccessory;
                    smkSubProdus.Add(objsubProdus);




                }
                smkProductView objsmkProductView = new smkProductView();

                smkAccessory objsmkAccessory = new smkAccessory();

                smkAccessory1 = getSmkAccessory1(id);
                smkAccessory2 = getSmkAccessory2(id);

                smkProductView.smkSubProdus = smkSubProdus;
                smkProductView.smkAccessory1 = smkAccessory1;
                smkProductView.smkAccessory2 = smkAccessory2;


                conn.Close();
            }
            smk_itens smk_itens = db.smk_itens.Find(id);
            smkProductView.smk_itensObj = smk_itens;

            return View(smkProductView);
        }
        public ActionResult deletaSubProdutos( string id1 ,string id2 ,string acao ,string idSmkItem)
        {
             switch (acao){
                    case "inserir":
                            
                         
                        break;

                    case "editar":
                            //$principal->Sub_produtos_materia_primas($_POST["id"], $_POST["codSubProduto"], $_POST["materia_primas_id"]);
                            //$principal->atualizaSub_produtos_materia_primas() == false ? $error = true : $error = false;
                        break;

                    case "apagar":
                            string str_query = @"delete from sub_produtos_materia_primas where sub_produtos_id = @sub_produtos_id
                                                and materia_primas_id = @materia_primas_id" ;               
                               
                            using (SqlConnection conn = new SqlConnection(str_connection))
                            using (SqlCommand cmd = new SqlCommand(str_query, conn))
                            {

                                cmd.Parameters.AddWithValue("@sub_produtos_id", Convert.ToInt32(id1));
                                cmd.Parameters.AddWithValue("@materia_primas_id", Convert.ToInt32(id2));

                                conn.Open();


                                cmd.ExecuteNonQuery();
                                conn.Close();
                                conn.Dispose();


                                conn.Close();
                            }
                         
                         break;

                    case "default":
                        
                        break;
            }
            return Json(new { ok = "success" });

        }

        [HttpGet]
        public ActionResult addSubProduct(string codSmk) {

            addSubProductView addSubProductViewObj = new addSubProductView();
            int codsmkInt = Convert.ToInt32(codSmk);
            smk_itens smk_itensObj = new smk_itens();

            smk_itensObj = (from x in db.smk_itens
                            where x.id ==  codsmkInt
                            select x
                            ).FirstOrDefault(); 



            return View(smk_itensObj);
        }


        public List<smkAccessory> getSmkProduct(int id)
        {
            List<smkAccessory> smkAccessory = new List<smkAccessory>();
            
            string str_query = @"SELECT *
												FROM sub_produtos_materia_primas spmp
												JOIN materia_primas mp ON mp.id = spmp.materia_primas_id
												WHERE sub_produtos_id = @id";

            using (SqlConnection conn = new SqlConnection(str_connection))
            using (SqlCommand cmd = new SqlCommand(str_query, conn))
            {
                cmd.Parameters.AddWithValue("@id", id);
                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    int nId = Convert.ToInt32(reader["id"]);
                    string descricao = reader["codigo_smk"].ToString() + "-" + reader["descricao"].ToString();
                    string unidade = reader["unidade"].ToString();
                    int sub_produtos_id = Convert.ToInt32(reader["sub_produtos_id"]);
                    int materia_primas_id = Convert.ToInt32(reader["materia_primas_id"]);


                    smkAccessory objsmkAccessory = new smkAccessory();


                    objsmkAccessory.id = nId;
                    objsmkAccessory.codigo_smk = descricao;
                    objsmkAccessory.unidade = unidade;
                    objsmkAccessory.sub_produtos_id = sub_produtos_id;
                    objsmkAccessory.materia_primas_id = materia_primas_id;
                    smkAccessory.Add(objsmkAccessory);

                }

                conn.Close();
            }


            return smkAccessory;
        }
        //
        public List<smkAccessory> getSmkAccessory1(int id)
        {
            List<smkAccessory> smkAccessory = new List<smkAccessory>();
           
            string str_query = @"SELECT *
														FROM acessorios ac
														JOIN smk_itens si ON si.id = ac.smk_itens_id
														JOIN materia_primas mp ON mp.id = ac.materia_primas_id 
														JOIN categoria_materia_primas cmp on cmp.id = mp.categoria_materia_primas_id
														where ac.smk_itens_id = @id order by cmp.descricao, mp.descricao";

            using (SqlConnection conn = new SqlConnection(str_connection))
            using (SqlCommand cmd = new SqlCommand(str_query, conn))
            {
                cmd.Parameters.AddWithValue("@id", id);
                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    int nId = Convert.ToInt32(reader["id"]);
                    string descricao = reader["codigo_smk"].ToString() + "-" + reader.GetString(10);
                    string unidade = reader["unidade"].ToString();

                    smkAccessory objsmkAccessory = new smkAccessory();
                    objsmkAccessory.id = nId;
                    objsmkAccessory.codigo_smk = descricao;
                    objsmkAccessory.unidade = unidade;
                    objsmkAccessory.quantitide = Convert.ToDecimal(reader["quantidade"]);
                    objsmkAccessory.categoria = reader.GetString(14);
                    smkAccessory.Add(objsmkAccessory);

                }

                conn.Close();
            }


            return smkAccessory;
        }
        // GET: /smkItmes/Create
        public List<smkAccessory> getSmkAccessory2(int id)
        {
            List<smkAccessory> smkAccessory = new List<smkAccessory>();

            string str_query = @"SELECT si.codigo_smk, mp.descricao, cmp.descricao AS categoria, mp.unidade as unidade, t1.sumQuantidade AS quantidade, mp.codigo_smk AS mp_codigo_smk
                                          FROM sub_produtos_materia_primas spmp
                                          JOIN sub_produtos sp ON sp.id = spmp.sub_produtos_id
                                          JOIN materia_primas mp ON mp.id = spmp.materia_primas_id
                                          JOIN categoria_materia_primas cmp ON cmp.id = mp.categoria_materia_primas_id
                                          JOIN smk_itens si ON si.id = sp.smk_itens_id
                                          JOIN (
                                         SELECT SUM( spmp.quantidade ) AS sumQuantidade, mp.codigo_smk AS codigo_smk
                                           FROM sub_produtos_materia_primas spmp
                                           JOIN materia_primas mp ON mp.id = spmp.materia_primas_id
                                          GROUP BY mp.codigo_smk ) t1 ON t1.codigo_smk = mp.codigo_smk
                                         WHERE si.id = @id
                                         ORDER BY cmp.descricao,mp.descricao";

            using (SqlConnection conn = new SqlConnection(str_connection))
            using (SqlCommand cmd = new SqlCommand(str_query, conn))
            {
                cmd.Parameters.AddWithValue("@id", id);
                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {

                    string descricao = reader["codigo_smk"].ToString() + "-" + reader["descricao"].ToString();
                    string unidade = reader["unidade"].ToString();
                    decimal quantidade = Convert.ToDecimal(reader["quantidade"]);
                    smkAccessory objsmkAccessory = new smkAccessory();

                    objsmkAccessory.codigo_smk = descricao;
                    objsmkAccessory.quantitide = quantidade;
                    objsmkAccessory.unidade = unidade;

                    smkAccessory.Add(objsmkAccessory);

                    smkAccessory.Add(objsmkAccessory);

                }

                conn.Close();


                return smkAccessory;
            }
        }
        public ActionResult Create()
        {
            return View();
        }

        public ActionResult addSubProdus(int id, int modelId) 
        {

            
            smk_itens smk_itensObj = new smk_itens();

            smk_itensObj = db.smk_itens.Find(modelId);

            smkaddSubProdutos smkaddSubProdutosObj = new smkaddSubProdutos();
            smkaddSubProdutosObj.smkItemObj = smk_itensObj;
            smkaddSubProdutosObj.cod = id;


            return View(smkaddSubProdutosObj);
        }

        //
        // POST: /smkItmes/Create

        [HttpPost]
        public ActionResult Create(smk_itens smk_itens)
        {
            if (ModelState.IsValid)
            {
                db.smk_itens.Add(smk_itens);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(smk_itens);
        }


        //
        // GET: /smkItmes/Edit/5

        public ActionResult Edit(int id)
        {
            smk_itens smk_itens = db.smk_itens.Find(id);
            return View(smk_itens);
        }

        //
        // POST: /smkItmes/Edit/5

        [HttpPost]
        public ActionResult Edit(smk_itens smk_itens)
        {
            if (ModelState.IsValid)
            {
                db.Entry(smk_itens).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(smk_itens);
        }

        //
        // GET: /smkItmes/Delete/5

        public ActionResult Delete(int id)
        {
            smk_itens smk_itens = db.smk_itens.Find(id);
            return View(smk_itens);
        }

        //
        // POST: /smkItmes/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            smk_itens smk_itens = db.smk_itens.Find(id);
            db.smk_itens.Remove(smk_itens);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}